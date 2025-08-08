// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2025-07-15",
  devtools: { enabled: false },
  components: true,
  plugins: [],
  css: ["vuetify/styles", '@mdi/font/css/materialdesignicons.css'],
  build: {
    transpile: ["vuetify"],
  },
});
