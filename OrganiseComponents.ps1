$componentsDir = "Components"

# Define the target folders and the files that belong in them
$layout = @{
    "Cards" = @("FixtureResultCard.razor", "OwnerCard.razor", "PoolCard.razor", "PoolTeamScoreCard.razor", "TeamCard.razor")
    "Navigation" = @("ColouredNavLink.razor", "PageHeader.razor")
    "DataDisplay" = @("BracketNode.razor", "FixtureRow.razor", "ScoreChip.razor")
    "Shared" = @("LoadingSpinner.razor")
}

# Ensure the base Components directory exists before running
if (-not (Test-Path $componentsDir)) {
    Write-Error "Could not find the 'Components' folder. Please run this script from the project root."
    exit
}

foreach ($folder in $layout.Keys) {
    $targetPath = Join-Path $componentsDir $folder
    
    # Create the subfolder if it doesn't exist
    if (-not (Test-Path $targetPath)) {
        New-Item -ItemType Directory -Path $targetPath | Out-Null
        Write-Host "Created: $targetPath" -ForegroundColor Green
    }

    # Move the files
    foreach ($file in $layout[$folder]) {
        # Check for the main .razor file, plus any .razor.css or .razor.cs files
        $extensions = @("", ".css", ".cs")
        
        foreach ($ext in $extensions) {
            $fileName = "$file$ext"
            $sourceFile = Join-Path $componentsDir $fileName
            
            if (Test-Path $sourceFile) {
                Move-Item -Path $sourceFile -Destination $targetPath
                Write-Host "  Moved -> $fileName to $folder\" -ForegroundColor Cyan
            }
        }
    }
}

Write-Host "`nComponent restructuring complete!" -ForegroundColor Green