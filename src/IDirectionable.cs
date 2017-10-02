///
/// An interface that ensures an object must have some cardinality associated
/// with it by implementing the Direction property.
///
interface IDirectionable
{
  Cardinality Direction { get; set; }
}
