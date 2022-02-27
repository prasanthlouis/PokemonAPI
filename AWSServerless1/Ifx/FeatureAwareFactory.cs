using PokemonAPI.Factories.AttackDescription;

namespace PokemonAPI.Ifx
{
    public class FeatureAwareFactory
    {
        private IAttackDescriptionFactory _attackDescriptionFactory;
        public FeatureAwareFactory(IAttackDescriptionFactory attackDescriptionFactory)
        {
            _attackDescriptionFactory = attackDescriptionFactory;
        }

        public IAttackDescriptionFactory CreateAttackDescriptionFactory()
        {
            return _attackDescriptionFactory.CreateAttackDescription();
        }
    }
}
