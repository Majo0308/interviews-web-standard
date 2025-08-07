// plugins/vuetify.ts
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import { VDateInput } from 'vuetify/labs/components' 
import { VCalendar } from 'vuetify/labs/VCalendar'

import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

export default defineNuxtPlugin((nuxtApp) => {
  const vuetify = createVuetify({
    components: {
      ...components, 
      VDateInput,   
      VCalendar
    },
    directives,
    icons: {
      defaultSet: 'mdi',
    },
  })
  nuxtApp.vueApp.use(vuetify)
})
