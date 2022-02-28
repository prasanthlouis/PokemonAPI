using PokemonAPI.Factories.AttackDescription;

namespace PokemonAPI.Ifx
{
    public interface IFeatureAwareFactory
    {
        IAttackDescription CreateAttackDescriptionFactory();
    }
    public class FeatureAwareFactory : IFeatureAwareFactory
    {
        private IAttackDescriptionFactory _attackDescriptionFactory;
        public FeatureAwareFactory(IAttackDescriptionFactory attackDescriptionFactory)
        {
            _attackDescriptionFactory = attackDescriptionFactory;
        }

        public IAttackDescription CreateAttackDescriptionFactory()
        {
            return _attackDescriptionFactory.CreateAttackDescription();
        }
    }
}
