# Stage 1: Build the Angular app
FROM node:14 as build

WORKDIR /app

# Copy the package.json and package-lock.json files to install dependencies
COPY package*.json ./

RUN npm install

# Copy the rest of the application source code
COPY . .

# Build the Angular app (replace 'your-app-name' with your app's name)
RUN npm run build --prod

# Stage 2: Create a lightweight production image
FROM nginx:latest

# Add a custom Nginx configuration to handle client-side routing
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy the built Angular app from the previous stage
COPY --from=build /app/dist/configuration-manager /usr/share/nginx/html

# Expose port 80 for the Nginx server
EXPOSE 80

# Start Nginx when the container is run
CMD ["nginx", "-g", "daemon off;"]
