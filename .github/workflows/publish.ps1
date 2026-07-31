# Stop on errors
$ErrorActionPreference = "Stop"

Write-Host "=== Running automated git push after publish ==="

# Ensure we're in the repo root
$repoRoot = git rev-parse --show-toplevel
Set-Location $repoRoot

# Stage all changes
git add -A

# Check if there is anything to commit
$changes = git status --porcelain

if ([string]::IsNullOrWhiteSpace($changes)) {
    Write-Host "No changes to commit. Skip commit and push."
} else {
	Write-Host "Changes to commit with Git."
}
exit 0

# Commit with timestamp
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
$commitMessage = "Automated publish: $timestamp"

Write-Host "Committing changes..."
git commit -m "$commitMessage"

# Check if remote is ahead
$localHash = git rev-parse HEAD
$remoteHash = git rev-parse origin/master

if ($localHash -ne $remoteHash) {
    Write-Host "Remote branch is ahead. Skipping push to avoid conflicts."
    exit 0
}

Write-Host "Pushing to origin..."
git push origin master

Write-Host "=== Automated publish complete ==="
