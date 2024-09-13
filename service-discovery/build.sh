#!/bin/bash
#Login to the ACR
az acr login --name $ACR_NAME

# Build the images
docker build -t $ACR_NAME.azurecr.io/service-discovery/checkout:latest ./checkout
docker build -t $ACR_NAME.azurecr.io/service-discovery/order-processor:latest ./order-processor

#Push the images
docker push $ACR_NAME.azurecr.io/service-discovery/checkout:latest
docker push $ACR_NAME.azurecr.io/service-discovery/order-processor:latest

#Update the deployment template file with both image tags and write it to a new file named deploy.yaml
sed -e "s/<order-processor-image>/$ACR_NAME.azurecr.io\/service-discovery\/order-processor:latest/g" \
    -e "s/<checkout-image>/$ACR_NAME.azurecr.io\/service-discovery\/checkout:latest/g" \
    ./deploy.yaml.template > deploy.yaml