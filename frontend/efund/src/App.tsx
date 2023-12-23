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
      main: '#EA5D0B',
    },
    secondary: {
      main: '#064663',
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
      default: '#041C32',
      paper: '#041C32',
    },
  },
});

const App: React.FC = () => (
  <ThemeProvider theme={darkTheme}>
    <CssBaseline />
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/sign-up" element={<SignUpPage />} />
        <Route path='/sign-in' element={<SignInPage />} />
        <Route path='/settings' element={(<><HomePage /><SettingsPage /></>)} />

        <Route path="*" element={<h1 style={{ color: 'red' }} >Not Found</h1>} />
      </Routes>
    </BrowserRouter>
  </ThemeProvider>

);

export default App;
