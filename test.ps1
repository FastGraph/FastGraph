$tag = "core/1.0.0";

$tagParts = $tag.split("/", 2);

# Full release
if ($tagParts.Length -eq 1)
{
"Full"
}
# Partial release
else
{
"Partial"
}


# # Retrieve MSBuild property name for which enabling package generation
# $tagSlug = $tagParts[0];
# $propertyName = GetPropertyNameFromSlug $tagSlug;
# $tagVersion = $tagParts[1];

# UpdatePackagesGeneration $propertyName;
# $env:Build_Version = $tagVersion;
# $projectName = $propertyName -replace "Generate_","";
# $projectName = $projectName -replace "_",".";
# $env:Release_Name = "$projectName $tagVersion";

# $env:IsFullIntegrationBuild = $env:Configuration -eq "Release";

