## v0.0.3
:watch: Released **12.12.2017**
---

:boom: **Breaking** All command arguments renamed to follow the more common pattern of `--arg|-a`:  
  For the `link-from` command:  
    `src` -> `--src|-s`  
	`as` -> `--name|-n`  
  For the `link-to` command:  
    `dest` -> `--dest|-d`  
	`link` -> `--link|-l`  
	`using` -> `--dlls|-D`  
  For the `restore` command:  
	`dest` -> `--dest|-d`  

:tada: **New** `help` command: `$ toffee help`  
:construction: **Maintainance** Refactored commands for better code re-use

## v0.0.2
:watch: Released **11.12.2017**
---

:bug: **Bugfix** Restore command rework, bug fixes, misc refactoring

## v0.0.1
:watch: Released **11.12.2017**
---

:tada: **New** All three main commands (`link-from`, `link-to`, `restore`) works
