using System;

namespace Tools.Patterns.Locator
{
    public interface ILocator
    {
        object GetResource(Type resourceType);
        TResource GetResource<TResource>();
    }
}