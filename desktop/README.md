# ERP TN Global Desktop Installer

This folder contains the Electron desktop app used to package the ERP TN Global project into a Windows installer.

## Build instructions

On a Windows machine:

```bash
cd desktop
npm install
npm run build
```

The generated installer files will be created in:

```text
desktop/dist
```

This setup generates:
- a portable executable
- a standard NSIS Windows installer (.exe)

## Notes

- The actual `.exe` installer is generated locally on a Windows machine.
- GitHub itself does not build the final installer binary.
- If you want a branded icon, add a file named `build/installer.ico` or update the builder config to reference your custom icon.
