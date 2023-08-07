namespace Abp.Localization.Avalonia.Core;

internal interface IUnsubscribe
{
    bool Unsubscribe(object observer);
}

internal interface IUnsubscribe<T> : IUnsubscribe
{
}
