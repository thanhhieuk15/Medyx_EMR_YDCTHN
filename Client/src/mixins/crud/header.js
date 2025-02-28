export default {
  props: ["title", "isSearching", "onlyTable", "actions", "paramHeader" ,"hasCheckbox","permission","hasExit"],
  methods: {
    exit() {
      // location.reload();
      location.href = `${window.origin}/HSBADS/Index`;
    },
    handleSearch() {
      this.$emit('search'); // Phát ra sự kiện tìm kiếm
    },
  },
  mounted() {
    window.addEventListener("keyup", (e) => {
      if (e.key === "F10") this.exit();
    });
  },
};
