param (
    [string]$Uri = "localhost:50051",
    [int]$InitialValue = 0,
    [string]$GhzWebUri = "http://localhost:8181"
)

Write-Output "Uri: ""$Uri"""
Write-Output "Initial Value: $InitialValue"

Write-Output "Creating counter..."
$create_counter_request = @"
{
    "initial_value": $InitialValue
}
"@
$create_counter_request = $create_counter_request.replace('"', '\"')
$create_counter_response = grpcurl -plaintext -d $create_counter_request  $Uri distributed_counter.protos.CounterService.CreateCounter | ConvertFrom-Json
$counter_id = $create_counter_response.counter_id
Write-Output "Created Counter: ""$counter_id"""

Write-Output "Running ghz benchmark..."
$increment_counter_request = @"
{
    "counter_id": "$counter_id",
    "increment_amount": 1
}
"@
$increment_counter_request = $increment_counter_request.replace('"', '\"')
$results = ghz --config ./increment_benchmark_config.json --data $increment_counter_request

Write-Output "Uploading results..."
$results | Invoke-WebRequest -Uri "$GhzWebUri/api/ingest" -Method POST -ContentType 'application/json' | Out-Null