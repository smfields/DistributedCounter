param (
    [string]$Uri = "localhost:50051",
    [int]$InitialValue = 0,
    [string]$GhzWebUri = "http://localhost:8181",
    [int]$ProjectId = 1,
    [string]$Tags = "{}"
)

Write-Output "Uri: ""$Uri"""
Write-Output "Initial Value: $InitialValue"

Write-Output "Running ghz benchmark..."
$create_counter_request = @"
{
    "initial_value": $InitialValue
}
"@
$create_counter_request = $create_counter_request.replace('"', '\"')
$results = ghz --config ./create_benchmark_config.json --data $create_counter_request --tags $Tags

Write-Output "Uploading results..."
$results | Invoke-WebRequest -Uri "$GhzWebUri/api/projects/$ProjectId/ingest" -Method POST -ContentType 'application/json' | Out-Null

Write-Output "Benchmark complete!"