namespace JsonLD.Entities
{
    /// <summary>
    /// Entity serializer
    /// </summary>
    public class EntitySerializer : IEntitySerializer
    {
        /// <summary>
        /// Deserializes the NQuads into a typed model
        /// </summary>
        /// <typeparam name="T">destination entity model type</typeparam>
        /// <param name="nQuads">RDF data in NQuads.</param>
        public T Deserialize<T>(string nQuads)
        {
            throw new System.NotImplementedException();
        }
    }
}
