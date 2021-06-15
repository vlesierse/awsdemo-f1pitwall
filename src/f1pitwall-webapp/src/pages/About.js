import Header from '../components/Header'

const About = () => {
  // page content
  const pageTitle = 'About'
  const pageDescription = 'welcome to react bootstrap template'

  return (
    <div>
      <Header head={pageTitle} description={pageDescription} />
    </div>
  )
}

export default About