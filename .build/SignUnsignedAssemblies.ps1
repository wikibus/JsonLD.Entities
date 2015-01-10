Import-Module ..\src\packages\Nivot.StrongNaming\tools\StrongNaming.psd1
$key = Import-StrongNameKeyPair ..\src\JsonLD.Entities\JsonLD.Entities.snk

dir ..\src\packages -rec *.dll | where { -not (test-strongname $_) } | set-strongname -keypair $key -verbose