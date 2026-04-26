#!/bin/bash
set -e 

# 1. Setup Build Container
container=$(buildah from mcr.microsoft.com/dotnet/sdk:10.0)
mountpoint=$(buildah mount $container)

# Copy the Backend folder into the container
# We copy from host './Backend' to container '/src'
buildah copy $container ./Backend /src

# 2. Run the Build
# We must set workingdir to where the .csproj actually lives
buildah run --workingdir /src/YouShelf $container dotnet publish -c Release -o /app/publish

# 3. Setup Runtime Container
runtime=$(buildah from mcr.microsoft.com/dotnet/aspnet:10.0)
runtime_mount=$(buildah mount $runtime)

# Move the artifacts from the build container mount to the runtime mount
mkdir -p $runtime_mount/app
cp -pPR $mountpoint/app/publish/. $runtime_mount/app/

# 4. Configure Runtime
# Based on your tree, the DLL name will be YouShelf.dll
buildah config --workingdir /app $runtime
buildah config --port 8080 $runtime
buildah config --entrypoint '["dotnet", "YouShelf.dll"]' $runtime

# 5. Commit and Cleanup
buildah commit $runtime youshelf-api:latest
buildah unmount $container
buildah unmount $runtime
buildah rm $container $runtime

echo "------------------------------------------------"
echo "Build Successful: youshelf-api:latest"
echo "Run locally with: podman run -p 8080:8080 youshelf-api:latest"