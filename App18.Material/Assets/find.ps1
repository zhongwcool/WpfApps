param(
    [string]$OutputFile = "$(Get-Date -Format 'yyyyMMddHHmmss').json"
)

# Function to extract MaterialDesign.Brush resources
function Extract-BrushResources {
    param (
        [string]$fileContents
    )
    $brushResources = @{}
    $matches = [regex]::Matches($fileContents, "MaterialDesign\.Brush\.[^\s\}]+")
    foreach ($match in $matches) {
        $resource = $match.Value
        if (-not $brushResources.ContainsKey($resource)) {
            $brushResources[$resource] = $resource
        }
    }
    return $brushResources
}

$allBrushResources = @{}

# Check if MigrateBrushes.ps1 exists in the current directory
$scriptFile = ".\MigrateBrushes.ps1"
if (Test-Path $scriptFile) {
    $fileContents = Get-Content $scriptFile -Encoding utf8BOM -Raw
    $brushResources = Extract-BrushResources -fileContents $fileContents
    $allBrushResources += $brushResources
} else {
    Write-Host "MigrateBrushes.ps1 not found in the current directory."
}

# Convert the brush resources to JSON and save to file
$allBrushResources | ConvertTo-Json | Set-Content -Path $OutputFile -Encoding utf8BOM