# Stop on errors
$ErrorActionPreference = "Stop"

Write-Host "=== Running automated git push after publish ==="

# Ensure we're in the repo root (Visual Studio runs publish from the project folder)
$repoRoot = git rev-parse --show-toplevel
Set-Location $repoRoot

# Stage all changes
git add -A

# Check if there is anything to commit
$changes = git status --porcelain

if ([string]::IsNullOrWhiteSpace($changes)) {
    Write-Host "No changes to commit. Skipping commit and push."
    exit 0
}

# Create a timestamped commit message
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
$commitMessage = "Automated publish: $timestamp"

Write-Host "Committing changes..."
git commit -m "$commitMessage"

Write-Host "Pushing to origin..."
git push origin HEAD

Write-Host "=== Automated publish complete ==="
