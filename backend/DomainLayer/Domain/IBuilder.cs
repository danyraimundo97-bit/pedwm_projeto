namespace DomainLayer.Domain
{
    /// <summary>Builds a product step-by-step; materialize in <see cref="Build"/>.</summary>
    public interface IBuilder<out TProduct>
    {
        TProduct Build();
    }
}
