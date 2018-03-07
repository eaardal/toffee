## v0.0.5
> :watch: Released **07.03.2018**

:bug: Fixed bug which caused the LinkRegistry.csv file to become corrupt

## v0.0.4
> :watch: Released **25.01.2018**

- Fixed `--dlls` param for the `link-to` command. It now works with `--dlls` and not `--dll`.
- Added wildcard search when using the `--dlls` parameter. Examples:
```
--dlls=Foo*
--dlls=Foo*,Bar
--dlls=Abc.dll,Bar,Foo*
```
You will be prompted to select the matching dlls you want to use before proceeding

## v0.0.3
> :watch: Released **12.12.2017**

:boom: **Breaking** All command arguments renamed to follow the more common pattern of `--arg|-a`:  
  - For the `link-from` command:  
    - `src` -> `--src|-s`  
    - `as` -> `--name|-n`  
  - For the `link-to` command:  
    - `dest` -> `--dest|-d`  
    - `link` -> `--link|-l`  
    - `using` -> `--dlls|-D`  
  - For the `restore` command:  
    - `dest` -> `--dest|-d`  

:tada: **New** `help` command: `$ toffee help`  
:construction: **Maintainance** Refactored commands for better code re-use

## v0.0.2
> :watch: Released **11.12.2017**

:bug: **Bugfix** Restore command rework, bug fixes, misc refactoring

## v0.0.1
> :watch: Released **11.12.2017**

:tada: **New** All three main commands (`link-from`, `link-to`, `restore`) works
