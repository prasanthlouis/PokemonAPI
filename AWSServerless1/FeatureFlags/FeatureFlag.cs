using PokemonAPI.Engines;
using PokemonAPI.Factories.AttackDescription;
using PokemonAPI.FeatureFlags.Providers;

namespace PokemonAPI.FeatureFlags
{

    public interface IFeatureFlag
    {
        IAttackDescriptionFeature _attackDescription { get; set; }
    }
    public class FeatureFlag : IFeatureFlag
    {
        public IAttackDescriptionFeature _attackDescription { get; set; }
        public FeatureFlag(IAttackDescriptionFeature attackDescription)
        {
            _attackDescription = attackDescription;
        }
    }
}
