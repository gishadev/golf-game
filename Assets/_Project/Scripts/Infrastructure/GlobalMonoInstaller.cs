using gishadev.golf.Core;
using Zenject;

namespace gishadev.golf.Infrastructure
{
    public class GlobalMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.BindInterfacesTo<GameManager>().AsSingle().NonLazy();
        }
    }
}