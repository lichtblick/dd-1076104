using OpenTelemetry.Resources;

namespace OtlpClient;

public class KubernetesResourceDetector : IResourceDetector
{
    public Resource Detect()
    {
        var attributes = new[]
        {
            GetAttributeFromEnv("k8s.namespace.name", "KUBE_NAMESPACE_NAME"),
            GetAttributeFromEnv("k8s.pod.uid", "KUBE_POD_UID"),
            GetAttributeFromEnv("k8s.pod.name", "KUBE_POD_NAME"),
            GetAttributeFromEnv("k8s.container.name", "KUBE_CONTAINER_NAME")
        }.Where(kv => kv != null).Select(kv => (KeyValuePair<string, object>)kv!);
        return new Resource(attributes);
    }

    private KeyValuePair<string, object>? GetAttributeFromEnv(string key, string envVariable)
    {
        var value = Environment.GetEnvironmentVariable(envVariable);
        return string.IsNullOrEmpty(value) ? null : new KeyValuePair<string, object>(key, value);
    }
}