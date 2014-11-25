namespace JsonLD.Entities
{
    /// <summary>
    /// Contract for serializing and de-serializing RDF resources
    /// </summary>
    public interface IEntitySerializer
    {
        /// <summary>
        /// Deserializes the NQuads into a typed model
        /// </summary>
        /// <typeparam name="T">destination entity model type</typeparam>
        /// <param name="nQuads">RDF data in NQuads.</param>
        T Deserialize<T>(string nQuads);
    }
}
