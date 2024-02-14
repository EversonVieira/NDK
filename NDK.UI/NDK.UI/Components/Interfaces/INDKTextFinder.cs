using NDK.UI.Models;

namespace NDK.UI.Components.Interfaces
{
    public interface INDKTextFinder<TOutput>
        where TOutput:NDKFinderOutput?
    {
        public Task<List<TOutput>> FindAsync(string? text = null, int? id = null, Func<TOutput, string>? TextExpression = null);

    }
}
