<template>
  <div class="navbar bg-base-100 shadow-sm">
    <!-- Navbar Start: Hamburger Menu and Title -->
    <div class="navbar-start">
      <!-- Hamburger Menu for Mobile -->
      <div class="dropdown">
        <div tabIndex="0" role="button" class="btn btn-ghost lg:hidden">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="h-5 w-5"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M4 6h16M4 12h8m-8 6h16"
            />
          </svg>
        </div>
        <ul
          tabIndex="0"
          class="dropdown-content menu menu-sm z-[1] mt-3 w-52 rounded-box bg-base-100 p-2 shadow"
        >
          <li><a href="#">Dashboard</a></li>
        </ul>
      </div>
      <!-- App Title -->
      <a class="btn btn-ghost text-xl">Expense Tracker</a>
    </div>
    <!-- Navbar Center: Desktop Menu -->
    <div class="navbar-center hidden lg:flex">
      <ul class="menu menu-horizontal px-1">
        <li><a href="#">Dashboard</a></li>
      </ul>
    </div>
    <!-- Navbar End: Theme Toggle -->
    <div class="navbar-end">
      <label class="flex cursor-pointer gap-2">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="20"
          height="20"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          stroke-linecap="round"
          stroke-linejoin="round"
        >
          <circle cx="12" cy="12" r="5" />
          <path
            d="M12 1v2M12 21v2M4.2 4.2l1.4 1.4M18.4 18.4l1.4 1.4M1 12h2M21 12h2M4.2 19.8l1.4-1.4M18.4 5.6l1.4-1.4"
          />
        </svg>
        <input
          type="checkbox"
          :checked="theme === 'dark'"
          @change="toggleTheme"
          class="theme-controller toggle"
        />
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="20"
          height="20"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          stroke-linecap="round"
          stroke-linejoin="round"
        >
          <path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"></path>
        </svg>
      </label>
    </div>
  </div>
</template>

<script>
import { defineComponent, ref, watch } from "vue";

export default defineComponent({
  name: "Navbar",
  // setup function (reactive state and functions to be used in the template)
  setup() {
    const theme = ref("light");

    // Initialize theme from localStorage or system preference
    if (
      localStorage.getItem("theme") === "dark" ||
      (!localStorage.getItem("theme") &&
        window.matchMedia("(prefers-color-scheme: dark)").matches)
    )
      theme.value = "dark";

    // Watch theme changes and update data-theme on <html>
    watch(theme, (newTheme) => {
      document.documentElement.setAttribute("data-theme", newTheme);
      localStorage.setItem("theme", newTheme);
    });

    // Apply initial theme
    document.documentElement.setAttribute("data-theme", theme.value);

    // Toggle theme function
    const toggleTheme = () => {
      theme.value = theme.value === "light" ? "dark" : "light";
    };

    return {
      theme,
      toggleTheme,
    };
  },
});
</script>
