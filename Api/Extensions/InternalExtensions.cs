namespace CIPP.Api.Extensions;

public static class InternalExtensions {
    public static RouteGroupBuilder Internal(this RouteGroupBuilder builder) {
        return builder.WithMetadata(new InternalAttribute());
    }
    
    public static bool IsInternal(this IList<object> metadata) {
        return metadata.OfType<InternalAttribute>().Any();
    }
}

public interface IInternalModule {
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class InternalAttribute : Attribute {
}
