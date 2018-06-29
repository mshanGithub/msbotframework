# Set version in AssemblyInfo.cs files.
Param (
    [string]$rootFolder, 
    [string]$version,
	[string]$filePattern = "AssemblyInfo.cs"
)

function UpdateAssemblyInfo()
{
    foreach ($file in $input) 
    {
        $path = $file.FullName
        Write-Host ($path)

        $patternAV = '^((?!\/\/).)*\[assembly: AssemblyVersion\("(.*)"\)\]'
        $patternAFV = '^((?!\/\/).)*\[assembly: AssemblyFileVersion\("(.*)"\)\]'
        (Get-Content $path) | ForEach-Object{
            if($_ -match $patternAV){
                # We have found the matching line
                '[assembly: AssemblyVersion("{0}")]' -f $version
            } elseif($_ -match $patternAFV){
                # We have found the matching line
                '[assembly: AssemblyFileVersion("{0}")]' -f $version
            } else {
                # Output line as is
                $_
            }
        } | Set-Content $path
    }        
}

Write-Host ("Root folder: " + $rootFolder)
Write-Host ("File pattern: " + $filePattern)
Write-Host ("Version: " + $version)

Write-Host ("Updating files...")
Get-Childitem -Path $rootFolder -recurse |? {$_.Name -like $filePattern} | UpdateAssemblyInfo; 
