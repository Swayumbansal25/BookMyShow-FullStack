/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        bmsPrimary: '#F84464', // BookMyShow Brand Red
        bmsDark: '#333545',
      }
    },
  },
  plugins: [],
}