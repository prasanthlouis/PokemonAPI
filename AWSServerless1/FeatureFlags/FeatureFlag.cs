using PokemonAPI.Engines;

namespace PokemonAPI.FeatureFlags
{

    public interface IFeatureFlag
    {
        IAttackDescription _attackDescription { get; set; }
    }
    public class FeatureFlag : IFeatureFlag
    {
        public IAttackDescription _attackDescription { get; set; }
        public FeatureFlag(IAttackDescription attackDescription)
        {
            _attackDescription = attackDescription;
        }
    }
}
