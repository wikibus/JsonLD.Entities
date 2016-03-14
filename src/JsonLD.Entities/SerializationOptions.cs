namespace JsonLD.Entities
{
    /// <summary>
    /// Represents various options, which modify the behavior of
    /// <see cref="EntitySerializer"/>
    /// </summary>
    public class SerializationOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity to be
        /// compacted with the current @context after being serialized
        /// </summary>
        public bool SerializeCompacted { get; set; }
    }
}