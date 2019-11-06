#tool paket:?package=OpenCover
#tool paket:?package=codecov
#tool paket:?package=gitlink
#addin paket:?package=Cake.Paket
#addin paket:?package=Cake.Codecov

var target = Argument("target", "Build");
var configuration = Argument("Configuration", "Debug");
var version = EnvironmentVariable("GitVersion_NuGetVersion") ?? "0.0.0";

Task("CI")
    .IsDependentOn("Pack")
    .IsDependentOn("Codecov").Does(() => {});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() => {
        PaketPack("nugets", new PaketPackSettings {
            Version = version
        });
    });

Task("Build")
    .Does(() => {
        DotNetCoreBuild("JsonLd.Entities.sln", new DotNetCoreBuildSettings {
            Configuration = configuration
        });
    })
    .DoesForEach(GetFiles("**/JsonLD.Entities.pdb"),
        pdb => GitLink3(pdb, new GitLink3Settings {
                RepositoryUrl = "https://github.com/wikibus/JsonLd.Entities",
            }));

Task("Codecov")
    .IsDependentOn("Test")
    .Does(() => {
        Codecov("opencover.xml");
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        var openCoverSettings = new OpenCoverSettings
        {
            OldStyle = true,
            MergeOutput = true,
            MergeByHash = true,
            Register = "user",
            ReturnTargetCodeOffset = 0
        }
        .WithFilter("+[JsonLD.Entities]*");

        bool success = true;
        foreach(var projectFile in new [] { "documentation.csproj", "jsonld.entities.tests.csproj" }.SelectMany(f => GetFiles($"**\\{f}")))
        {
            try
            {
                openCoverSettings.WorkingDirectory = projectFile.GetDirectory();

                OpenCover(context => {
                        context.DotNetCoreTool(
                          projectPath: projectFile.FullPath,
                          command: "xunit",
                          arguments: $"-noshadow");
                    },
                    "opencover.xml",
                    openCoverSettings);
            }
            catch(Exception ex)
            {
                success = false;
                Error("There was an error while running the tests", ex);
            }
        }

        if(success == false)
        {
            throw new CakeException("There was an error while running the tests");
        }
    });

RunTarget(target);
