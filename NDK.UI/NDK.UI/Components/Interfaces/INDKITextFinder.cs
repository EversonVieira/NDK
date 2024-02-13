using NDK.UI.Models;

namespace NDK.UI.Components.Interfaces
{
    public interface INDKITextFinder<TOutput>
        where TOutput:NDKFinderOutput
    {
        public List<TOutput> Find(string? text = null, int? id = null);

    }
}
