<template>
  <div>
    <Navbar />
    <Dashboard />
    <Footer />
  </div>
</template>

<script>
import Navbar from './components/Navbar.vue';
import Footer from './components/Footer.vue';
import Dashboard from './components/Dashboard.vue';
export default {
  components: {
    Navbar,
    Footer,
    Dashboard,
  },
};
</script>
