# Toffee

> Npm link for .NET

Replaces DLL references to NuGet packages in `.csproj` files with local versions instead, to make life easier when developing NuGet packages used by other applications and libraries.

[Download](https://github.com/eaardal/toffee/releases)

# How to use

## `link-from`

```
$ toffee link-from --src={path} --name={link-name}
```

### Args

#### `--src` | `-s`

Path to directory containing DLL's you want to use in another project. Typically
a `bin/Debug` directory.

**Example**

```
--src=C:\projects\Foo\Foo.Project\bin\Debug
-s=C:\projects\Foo\Foo.Project\bin\Debug
```

:exclamation: You might get the error `Path to {--src|-s} directory does not exist. Remember to wrap the path in double quotes if it contains spaces.` even though you have a 100% correct path. In that case, this error is probably due to wrong slashes in your path, depending on what terminal you use.
If your path is `C:\foo\bar` try `C:/foo/bar` instead, or vice versa. This will get fixed in the future :sweat:

#### `--name` | `-n`

Name of the link pointing to the `src` directory.

* Can not contain spaces

**Example**

```
--name=foo-project
-n=foo-project
```

> * Links are stored in `%appdata%\Toffee\LinkRegistry.csv`
> * Links are overwritten with new {src} path if it already exists

## `link-to`

```
$ toffee link-to --dest={path} --link={link-name} --dlls={dlls}
```

### Args

#### `--dest` | `-d`

Path to the project directory where you want to use the DLL's from a link you've
made, instead of the original NuGet reference. Typically the project's git root
directory, or the same directory your `.sln` lives.

**Example**

```
--dest=C:\projects\Bar
-d=C:\projects\Bar
```

:exclamation: You might get the error `Path to {--dest|-d} directory does not exist. Remember to wrap the path in double quotes if it contains spaces.` even though you have a 100% correct path. In that case, this error is probably due to wrong slashes in your path, depending on what terminal you use.
If your path is `C:\foo\bar` try `C:/foo/bar` instead, or vice versa. This will get fixed in the future :sweat:

#### `--link` | `-l`

Name of the link to use, as entered when using the `link-from` command.

**Example**

```
--link=foo-project
-l=foo-project
```

#### `--dlls` | `-D`

Comma separated list of DLL's, to replace in csprojs (found recursively under
the `dest` directory) with DLL's found in the named link's `src` directory
instead.

- The DLL name can also contain wildcards: `Foo.*`
- The DLL name can end without the `.dll` extension: `--dlls=MyProject`

**Example**

```
--dlls=Foo.Project.dll,Abc.Project,Bar*
-D=Foo.Project.dll,Abc.Project.dll
```

The two DLL's will be concatenated with the link's `--src` directory to make a
complete path: `C:\projects\Foo\Foo.Project\bin\Debug\Foo.Project.dll` and
`C:\projects\Foo\Foo.Project\bin\Debug\Abc.Project.dll`. These paths will
replaced in all csprojs under the `dest` directory where `Foo.Project.dll` and
`Abc.Project.dll` is referenced.

## `restore`

```
$ toffee restore --dest={path}
```

### Args

#### `--dest` | `-d`

Path to the project directory you want to restore. Typically the project's git
root directory, or the same directory your `.sln` lives.

**Example**

```
--dest=C:\projects\Bar
-d=C:\projects\Bar
```

## `help`

```
$ toffee help
```

Displays information about each command and their arguments

## `get-links`

```
$ toffee get-links
```

Lists all links

# Dev stuff

```
dotnet publish Toffee.sln -c Release -f netcoreapp2.0 -r win10-x64 --self-contained
```
