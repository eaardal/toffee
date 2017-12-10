# Toffee

## `link-from`

```
$ toffee link-from src={path} as={link-name}
```

### Args

#### `src`

Path to directory containing DLL's you want to use in another project. Typically
a `bin/Debug` directory.

**Example**

```
src=C:\projects\Foo\Foo.Project\bin\Debug
```

#### `as`

Name of the link pointing to the `src` directory.

* Can not contain spaces

**Example**

```
as=foo-project
```

> Links are stored in `%appdata%\Toffee\LinkRegistry.csv`

## `link-to`

```
$ toffee link-to dest={path} link={link-name} using={dlls}
```

### Args

#### `dest`

Path to the project directory where you want to use the DLL's from a link you've
made, instead of the original NuGet reference. Typically the project's git root
directory, or the same directory your `.sln` lives.

**Example**

```
dest=C:\projects\Bar
```

#### `link`

Name of the link to use, as entered when using the `link-from` command.

**Example**

```
link=foo-project
```

#### `using`

Comma separated list of DLL's, to replace in csprojs (found recursively under
the `dest` directory) with DLL's found in the named link's `src` directory
instead.

**Example**

```
using=Foo.Project.dll,Abc.Project.dll
```

The two DLL's will be concatenated with the link's `src` directory to make a
complete path: `C:\projects\Foo\Foo.Project\bin\Debug\Foo.Project.dll` and
`C:\projects\Foo\Foo.Project\bin\Debug\Abc.Project.dll`. These paths will
replaced in all csprojs under the `dest` directory where `Foo.Project.dll` and
`Abc.Project.dll` is referenced.

## `restore`

```
$ toffee restore dest={path}
```

### Args

#### `dest`

Path to the project directory you want to restore. Typically the project's git
root directory, or the same directory your `.sln` lives.

**Example**

```
dest=C:\projects\Bar
```

## Dev stuff

```
dotnet publish Toffee.sln -c Release -f netcoreapp2.0 -r win10-x64 --self-contained
```
