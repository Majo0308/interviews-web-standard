<!-- <v-row align="center">
              <v-col cols="auto">
                <v-btn
                  size="small"
                  icon="mdi-clipboard-text-clock-outline"
                  @click="editTask(task)"
                  :style="
                    task.estadoId === 1
                      ? {
                          color: 'white',
                          backgroundColor: '#F06565',
                        }
                      : {}
                  "
                  class="ma-0, mr-2"
                ></v-btn>
                <v-btn
                  size="small"
                  icon="mdi-clipboard-text-play-outline"
                  @click="deleteTask(task)"
                  class="ma-0 mr-2"
                  :style="
                    task.estadoId === 1
                      ? {
                          color: 'white',
                          backgroundColor: '#f0b356',
                        }
                      : {}
                  "
                ></v-btn>
                <v-btn
                  size="small"
                  icon="mdi-clipboard-check-outline"
                  @click="editTask(task)"
                  class="ma-0"
                  style="color: white; background-color: #5fce73"
                ></v-btn>
              </v-col>
              <v-col>
                <v-chip
                  v-for="tag in task.tags"
                  :key="tag.tagId"
                  class="mr-1"
                  :color="tag.tagColor || 'primary'"
                  text-color="white"
                  small
                >
                  {{ tag.tagName }}
                </v-chip>
              </v-col>
              <v-col cols="auto"> {{ task.taskDueDate }}</v-col>
            </v-row> -->