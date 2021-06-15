const Footer = () => {
  const year = new Date().getFullYear()
  return (
    <footer className='text-center text-capitalize'>
      copyright F1 Pit Wall &copy; {year}
    </footer>
  )
}

export default Footer