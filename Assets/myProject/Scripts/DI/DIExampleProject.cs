using UnityEngine;

namespace DI
{
    public class MyTestProjectService { }
    public class MyTestSceneService
    {
        private readonly MyTestProjectService _myTestProjectService;

        public MyTestSceneService(MyTestProjectService myTestProjectService)
        {
            _myTestProjectService = myTestProjectService;
        }
    }

    public class MyTestFactory
    {
        public MyTestObject CreateInstance(string id, int par1) => new MyTestObject(id, par1);
    }
    public class MyTestObject
    {
        private readonly string _id;
        private readonly int _par1;

        public MyTestObject(string id, int par1)
        {
            _id = id;
            _par1 = par1;
        }
        public override string ToString() => $"Object with id: {_id} and par1: {_par1}";
    }

    public class DIExampleProject : MonoBehaviour
    {
        private void Awake()
        {
            var projectContainer = new DIContainer();
            projectContainer.RegisterSingleton(_ => new MyTestProjectService());
            projectContainer.RegisterSingleton("option 1", _ => new MyTestProjectService());//AllEnums.ProjectTags.tag1.ToString()
            projectContainer.RegisterSingleton("option 2", _ => new MyTestProjectService());

            FindFirstObjectByType<DIExampleScene>()?.Init(projectContainer);
        }
    }

    public class DIExampleScene : MonoBehaviour
    {

        public void Init(DIContainer projectContainer)
        {
            //ex: use project lvl for resolve (work but better use sceneContainer)
            var serviceWithoutTag = projectContainer.Resolve<MyTestProjectService>();
            var service1 = projectContainer.Resolve<MyTestProjectService>("option 1");
            var service2 = projectContainer.Resolve<MyTestProjectService>("option 1");

            //ex: use scene lvl for resolve
            var sceneContainer = new DIContainer(projectContainer);
            sceneContainer.RegisterSingleton(container => new MyTestSceneService(container.Resolve<MyTestProjectService>()));
            sceneContainer.RegisterSingleton(_ => new MyTestFactory());
            sceneContainer.RegisterInstance(new MyTestObject("testId", 10));

            var objectFactory = sceneContainer.Resolve<MyTestFactory>();
            for (int i = 0; i < 3; i++)
            {
                var id = $"o{i}";
                var obj = objectFactory.CreateInstance(id, i);
                Debug.Log($"Created: {obj}");
            }

        }
    }

    //public class AllEnums
    //{
    //    public enum ProjectTags
    //    {
    //        tag1,
    //        tag2,
    //    }
    //}
}
