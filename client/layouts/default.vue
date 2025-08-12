<script setup lang="ts">
import { provide, ref, onMounted, watch } from "vue";
import { useDisplay } from "vuetify";
import { fetchGetApi } from "~/server/api";
import type { TagType } from "~/server/api-schema";

const { mdAndDown } = useDisplay();
const drawerOpen = ref(false);
const tags = ref<TagType[]>([]);
const selectedTag = ref<number | null>(null);
const selectedMenu = ref<string | number | null>('all-tasks');
provide("tags", tags);
provide("selectedTag", selectedTag);

onMounted(() => {
  getTags();
  drawerOpen.value = !mdAndDown.value;
});

watch(mdAndDown, (val) => {
  drawerOpen.value = !val;
});

const selectTag = (id: number) => {
  selectedMenu.value = id == 0 ? "all-tasks" : `tag-${id}`;
  selectedTag.value = id;
  navigateTo("/");
  if (mdAndDown.value) drawerOpen.value = false;
};

const getViewCalendar = async () => {
  selectedMenu.value = "calendar";
  navigateTo("/calendar");
  if (mdAndDown.value) drawerOpen.value = false;
};

const getViewTag = async () => {
  selectedMenu.value = "tags";
  navigateTo("/tags");
  if (mdAndDown.value) drawerOpen.value = false;
};

const getViewSubtasks = async () => {
  selectedMenu.value = "subtasks";
  navigateTo("/subtasks");
  if (mdAndDown.value) drawerOpen.value = false;
};

const getTags = async () => {
  try {
    const response = await fetchGetApi<TagType[]>("/Tags");
    tags.value = response;
  } catch (err) {
    console.error("Error fetching tags:", err);
  }
};


provide("refreshTags", getTags);
</script>

<template>
  <v-app>
    <v-app-bar app style="background-color: #323ab3; color: white">
      <v-app-bar-nav-icon v-if="mdAndDown" @click="drawerOpen = !drawerOpen" />
      <v-toolbar-title>My Task App</v-toolbar-title>
    </v-app-bar>

    <v-navigation-drawer
      app
      v-model="drawerOpen"
      :permanent="!mdAndDown"
      :temporary="mdAndDown"
      width="280"
    >
      <v-list>
        <v-list-item
          @click="selectTag(0)"
          :active="selectedMenu === 'all-tasks'"
        >
          <v-list-item-title>ALL TASKS</v-list-item-title>
        </v-list-item>

        <v-list-item @click="selectTag(-1)" :active="selectedMenu === 'tag--1'">
          <v-list-item-title>TASK TODAY</v-list-item-title>
        </v-list-item>

        <v-list-item
          @click="getViewSubtasks()"
          :active="selectedMenu === 'subtasks'"
        >
          <v-list-item-title>ALL SUBTASKS</v-list-item-title>
        </v-list-item>

        <v-list-item @click="getViewTag()" :active="selectedMenu === 'tags'">
          <v-list-item-title>TAGS</v-list-item-title>
        </v-list-item>

        <v-list-item
          @click="getViewCalendar()"
          :active="selectedMenu === 'calendar'"
        >
          <v-list-item-title>CALENDAR</v-list-item-title>
        </v-list-item>

        <v-divider></v-divider>

        <v-list-item
          v-for="tag in tags"
          :key="tag.tagId"
          :value="tag.tagId"
          @click="selectTag(tag.tagId)"
          :active="selectedMenu === `tag-${tag.tagId}`"
        >
          <v-list-item-title>{{ tag.tagName }}</v-list-item-title>
        </v-list-item>


      </v-list>
    </v-navigation-drawer>

    <!-- Main content -->
    <v-main>
      <slot />
    </v-main>
  </v-app>
</template>
