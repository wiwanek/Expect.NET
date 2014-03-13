properties { 
  $base_dir  = resolve-path .
  $lib_dir = "$base_dir\SharedLibs"
  $build_dir = "$base_dir\build" 
  $buildartifacts_dir = "$build_dir\" 
  $sln_file = "$base_dir\Expect.NET.sln" 
  $major = 2
  $minor = 0
  $patch = 0
  $build = 0
  $label = "beta"
  $version_assembly = "$major.$minor.$patch.$build"
  $version_nuget = "$major.$minor.$patch"
  if ( $label -ne "" ) {
	$pattern = "^[a-zA-Z0-9.]+$"
	if ($label -match $pattern) {
		$version_nuget = "$version_nuget-$label"
	} else {
		throw "Label must comprise only ASCII alphanumerics and dot [a-zA-Z0-9.]"
	}
  }
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
	UpdateVersion
} 

task Compile -depends Init { 
  Framework '4.5'
  msbuild /p:OutDir=$buildartifacts_dir /p:BuildProjectReferences=false $base_dir\Expect.NET\Expect.csproj
  msbuild /p:OutDir=$buildartifacts_dir /p:BuildProjectReferences=false $base_dir\Expect.Test\Expect.Test.csproj 
  msbuild /p:OutDir=$buildartifacts_dir /p:BuildProjectReferences=false $base_dir\ExampleApp\ExampleApp.csproj
} 

task Test -depends Compile {
 # $old = pwd
 # cd $build_dir
    exec { & MSTest /testcontainer:$buildartifacts_dir\Expect.Test.dll } 
  #cd $old        
}

task Release -depends Test {
	copy-item $build_dir\Expect.NET.dll $release_dir
	exec { & $nuget pack "$specFile" -Version "$version_nuget" -OutputDirectory "$release_dir" }
}

function UpdateVersion {
	Update-AssemblyInfoFiles $version_assembly
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

