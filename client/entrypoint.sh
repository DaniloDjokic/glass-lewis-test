#!/bin/sh

# Replace the placeholder with actual API URL
find /usr/share/nginx/html -name "*.js" -exec sed -i "s|API_URL_PLACEHOLDER|${ApiUrl}|g" {} +


exec "$@"
