#!/bin/bash
#Login to the ACR
az acr login --name $ACR_NAME

# Build the images
docker build -t $ACR_NAME.azurecr.io/service-discovery/state-store:latest .

#Push the images
docker push $ACR_NAME.azurecr.io/service-discovery/state-store:latest

#Update the deployment template file with both image tags and write it to a new file named deploy.yaml
sed -e "s/<state-store-image>/$ACR_NAME.azurecr.io\/service-discovery\/state-store:latest/g" \
    ./deploy.yaml.template > deploy.yaml