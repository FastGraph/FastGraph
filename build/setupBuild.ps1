# Update .props based on git tag status & setup build version
if ($env:APPVEYOR_REPO_TAG -eq "true")
{
    $tagParts = $env:APPVEYOR_REPO_TAG_NAME.split("/", 2);

    # Full release
    if ($tagParts.Length -eq 1) # X.Y.Z
    {
        $env:Build_Version = $env:APPVEYOR_REPO_TAG_NAME;
        $env:Release_Name = $env:Build_Version;
    }
    # Partial release
    else # Slug/X.Y.Z
    {

    }

    $env:IsFullIntegrationBuild = $false;   # Run only tests on deploy builds (not coverage, etc.)
}
else
{
    $env:Build_Version = "$($env:APPVEYOR_BUILD_VERSION)";
    $env:Release_Name = $env:Build_Version;

    $env:IsFullIntegrationBuild = "$env:APPVEYOR_PULL_REQUEST_NUMBER" -eq "" -And $env:Configuration -eq "Release";
}

$env:Build_Assembly_Version = "$env:Build_Version" -replace "\-.*","";

"Building version: $env:Build_Version";
"Building assembly version: $env:Build_Assembly_Version";

if ($env:IsFullIntegrationBuild -eq $true)
{
    "With full integration";

    $env:PATH="C:\Program Files\Java\jdk15\bin;$($env:PATH)"
    $env:JAVA_HOME_11_X64='C:\Program Files\Java\jdk15'
    $env:JAVA_HOME='C:\Program Files\Java\jdk15'
}
else
{
    "Without full integration";
}