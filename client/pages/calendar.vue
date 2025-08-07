<template>
  <div>
    <v-sheet
      class="d-flex"
      height="54"
      tile
    >
      <v-select
        v-model="type"
        :items="types"
        class="ma-2"
        density="compact"
        label="View Mode"
        variant="outlined"
        hide-details
      ></v-select>
      <v-select
        v-model="weekday"
        :items="weekdays"
        class="ma-2"
        density="compact"
        label="weekdays"
        variant="outlined"
        hide-details
      ></v-select>
    </v-sheet>
    <v-sheet>
      <v-calendar
        ref="calendar"
        v-model="value"
        :events="events"
        :view-mode="type"
        :weekdays="weekday"
      ></v-calendar>
    </v-sheet>
  </div>
</template>

<script>
import { useDate } from 'vuetify'
import { fetchGetApi } from '~/server/api'

export default {
  data: () => ({
    type: 'month',
    types: ['month', 'week', 'day'],
    weekday: [0, 1, 2, 3, 4, 5, 6],
    weekdays: [
      { title: 'Sun - Sat', value: [0, 1, 2, 3, 4, 5, 6] },
      { title: 'Mon - Sun', value: [1, 2, 3, 4, 5, 6, 0] },
      { title: 'Mon - Fri', value: [1, 2, 3, 4, 5] },
      { title: 'Mon, Wed, Fri', value: [1, 3, 5] },
    ],
    value: [new Date()],
    events: [],
  }),
  mounted () {
    const adapter = useDate()
    this.getEvents({
      start: adapter.startOfDay(adapter.startOfMonth(new Date())),
      end: adapter.endOfDay(adapter.endOfMonth(new Date()))
    })
  },
  methods: {
    async getEvents ({ start, end }) {
      try {
        const tasks = await fetchGetApi('/Tasks')
        const events = tasks.map(task => {
          const startDate = new Date(task.taskDueDate)
          const endDate = new Date(task.taskDueDate)

          return {
            title: task.taskName,
            start: startDate,
            end: endDate,
            color: task.taskPriority === 2? 'red' : 'blue',
            allDay: true,
          }
        })

        this.events = events
      } catch (error) {
        console.error('Error fetching tasks:', error)
      }
    },
  },
}
</script>
