using System.Collections.Generic;
using System;

namespace DI
{
    public class DIContainer
    {
        private readonly DIContainer _parentContainer;
        private readonly Dictionary<(string, Type), DIRegistration> _registations = new();
        ///For  catch dependency looping
        private readonly HashSet<(string,Type)> _resolutions = new();

        public DIContainer(DIContainer parentContainer = null)
        {
            _parentContainer = parentContainer;
        }

        public T Resolve<T>(string tag = null)
        {
            var key = (tag, typeof(T));

            if (_resolutions.Contains(key))
                throw new Exception(
                    $"DI: looping dependency for tag {tag} and type {key.Item2.FullName}");
            _resolutions.Add(key);

            try
            {

                if (_registations.TryGetValue(key, out var registration))
                {
                    if (registration.IsSingleton)
                    {
                        if (registration.Instance == null && registration.Factory != null)
                            registration.Instance = registration.Factory(this);

                        return (T)registration.Instance;
                    }
                    else
                        return (T)registration.Factory(this);

                }
                else if (_parentContainer != null)
                {
                    return _parentContainer.Resolve<T>(tag);
                }
                else
                    throw new Exception(
                        $"DI: Coudnt find dependency for tag {tag} and type {key.Item2.FullName}");
            }
            finally
            {
                _resolutions.Remove(key);
            }
        }

        public void RegisterSingleton<T>(Func<DIContainer, T> factory)
        {
            RegisterSingleton(null, factory);
        }

        ///Register one instance for each (tag,T)
        public void RegisterSingleton<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            Register(key, factory, true);
        }

        public void RegisterTransient<T>(Func<DIContainer, T> factory)
        {
            RegisterTransient(null, factory);
        }

        ///Register without cashed instance (on resolve => always new instance)
        public void RegisterTransient<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            Register(key, factory, false);
        }

        public void RegisterInstance<T>(T instance)
        {
            RegisterInstance(null, instance);
        }

        ///Register created outside instance
        public void RegisterInstance<T>(string tag, T instance)
        {
            var key = (tag, typeof(T));

            if (_registations.ContainsKey(key))
                throw new Exception(
                    $"DI: Factory with tag {tag} and type {typeof(T)} has already registered");

            _registations[key] = new DIRegistration
            {
                Instance = instance,
                IsSingleton = false
            };
        }

        public void Register<T>((string, Type) key, Func<DIContainer, T> factory, bool isSingleton)// where T : object
        {
            if (_registations.ContainsKey(key))
                throw new Exception(
                    $"DI: Factory with tag {key.Item1} and type {key.Item2.FullName} has already registered");

            _registations[key] = new DIRegistration
            {
                Factory = result => factory(result),
                IsSingleton = isSingleton
            };
        }
    }
}