import './App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage';
import SignUpPage from './pages/SignUpPage';
import SignInPage from './pages/SignInPage';
import { CssBaseline, ThemeProvider, createTheme } from '@mui/material';
import SettingsPage from './pages/SettingsPage';

const darkTheme = createTheme({
  palette: {
    mode: 'dark',
    primary: {
      main: '#ff7654',
      // dark: '#ff5f38',
      // light: '#fe8f72',
      contrastText: '#272525',
    },
    secondary: {
      main: '#064663',
      contrastText: '#fff',
    },
    error: {
      main: '#D61F3D',
    },
    info: {
      main: '#00A7E1  ',
    },
    success: {
      main: '#4CAF50',
    },
    warning: {
      main: '#FF9800',
    },
    background: {
      default: '#141313',
      paper: '#1E1E1E',
    },
  },
});

const lightTheme = createTheme({
  palette: {
    primary: {
      main: '#ff7654',
      contrastText: '#ebf2fa',
    },
    // secondary: {
    //   main: '#25a18e',
    //   contrastText: '#ebf2fa',
    // },
    // error: {
    //   main: '#D61F3D',
    // },
    // info: {
    //   main: '#00A7E1  ',
    // },
    // success: {
    //   main: '#4CAF50',
    // },
    // warning: {
    //   main: '#FF9800',
    // },
    background: {
      default: '#ebf2fa',
      paper: '#e2e2e2',
    },
    // text: {
    //   primary: '#041C32',
    //   secondary: '#041C32',
    // }
  },
});

const App: React.FC = () => (
  <ThemeProvider theme={lightTheme}>
    <CssBaseline />
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/sign-up" element={<SignUpPage />} />
        <Route path='/sign-in' element={<SignInPage />} />
        <Route path='/settings' element={(<SettingsPage />)} />

        <Route path="*" element={<h1 style={{ color: 'red' }} >Not Found</h1>} />
      </Routes>
    </BrowserRouter>
  </ThemeProvider>

);

export default App;
