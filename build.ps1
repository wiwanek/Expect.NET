properties { 
  $base_dir  = resolve-path .
  $lib_dir = "$base_dir\SharedLibs"
  $build_dir = "$base_dir\build" 
  $buildartifacts_dir = "$build_dir\" 
  $sln_file = "$base_dir\Expect.NET.sln" 
  $major = 1
  $minor = 1
  $build = 39
  $revision = 0
  $version = "$major.$minor.$build.$revision"
  $nuget = "$base_dir\.nuget\nuget.exe"
  $release_dir = "$base_dir\Release"
  $spec_file = "Expect.nuspec"
} 

task default -depends Test

task Clean { 
  remove-item -force -recurse $buildartifacts_dir
  remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue 
} 

task Init -depends Clean { 
    new-item $release_dir -itemType directory 
    new-item $buildartifacts_dir -itemType directory 
} 

task Compile -depends Init { 
  Framework '4.5'
  msbuild /p:OutDir=$buildartifacts_dir /p:BuildProjectReferences=false $base_dir\Expect.NET\Expect.csproj
  msbuild /p:OutDir=$buildartifacts_dir /p:BuildProjectReferences=false $base_dir\Expect.Test\Expect.Test.csproj 
  msbuild /p:OutDir=$buildartifacts_dir /p:BuildProjectReferences=false $base_dir\ExampleApp\ExampleApp.csproj
  msbuild /p:BuildProjectReferences=false $base_dir\ExampleAppNuget\ExampleAppNuget.csproj
  StepVersion
} 

task Test -depends Compile {
 # $old = pwd
 # cd $build_dir
    exec { & MSTest /testcontainer:$buildartifacts_dir\Expect.Test.dll } 
  #cd $old        
}

task Release -depends Test {
	copy-item $build_dir\Expect.NET.dll $release_dir
	exec { & $nuget pack "$specFile" -Version "$version" -OutputDirectory "$release_dir" }
}

function StepVersion {
	$build++
	$version = "$major.$minor.$build.$revision"
	Update-AssemblyInfoFiles $version
	Update-CurrentScriptFile $build
}

#-------------------------------------------------------------------------------
# Update version numbers of AssemblyInfo.cs
#-------------------------------------------------------------------------------
function Update-AssemblyInfoFiles ([string] $version) {
    $assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $assemblyVersion = 'AssemblyVersion("' + $version + '")';
    $fileVersion = 'AssemblyFileVersion("' + $version + '")';
     
    Get-ChildItem -r -filter AssemblyInfo.cs | ForEach-Object {
        $filename = $_.Directory.ToString() + '\' + $_.Name
        $filename + ' -> ' + $version
         
        (Get-Content $filename) | ForEach-Object {
            % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
            % {$_ -replace $fileVersionPattern, $fileVersion }
        } | Set-Content $filename
    }
}

#-------------------------------------------------------------------------------
# Update version numbers of current script
#-------------------------------------------------------------------------------
function Update-CurrentScriptFile ([int] $build) {
    $buildPattern = '\$build = [0-9]+'
    $buildStr = '$build = ' + $build;

    $filename = "build.ps1"
    	
	$filename + ' -> ' + $build
    
	(Get-Content $filename) | ForEach-Object {
        % {$_ -replace $buildPattern, $buildStr } 
    } | Set-Content $filename
}
