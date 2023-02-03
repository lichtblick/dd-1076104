1. deploy datadog helm chart with [datadog-3.7.3/values.yaml](datadog-3.7.3/values.yaml) in namespace datadog (requires existing secret "``datadog``" and the ``clusterName`` must be changed or removed)
2. deploy opentelemetry collector with [opentelemetry-collector-0.47.0/values.yaml](opentelemetry-collector-0.47.0/values.yaml) in namespace datadog
3. build and push docker image in [OtlpClient/](OtlpClient/)
4. adjust repository in [deployment.yaml](deployment.yaml) and deploy to k8s