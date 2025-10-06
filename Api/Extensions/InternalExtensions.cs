namespace CIPP.Api.Extensions;

public static class InternalExtensions {
    public static RouteGroupBuilder Internal(this RouteGroupBuilder builder) {
        return builder.WithMetadata(new InternalAttribute());
    }
    
    public static RouteGroupBuilder ExcludeFromVersioning(this RouteGroupBuilder builder) {
        return builder.WithMetadata(new ExcludeFromVersioningAttribute());
    }
    
    public static bool IsInternal(this IList<object> metadata) {
        return metadata.OfType<InternalAttribute>().Any();
    }
    
    public static bool IsExcludedFromVersioning(this IList<object> metadata) {
        return metadata.OfType<ExcludeFromVersioningAttribute>().Any();
    }
}

public interface IInternalModule {
}

public interface IVersioningExcluded {
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class InternalAttribute : Attribute {
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ExcludeFromVersioningAttribute : Attribute {
}
